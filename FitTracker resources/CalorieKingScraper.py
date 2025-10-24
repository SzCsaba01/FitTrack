import time
import json
import csv
import os
from selenium import webdriver
from selenium.webdriver.common.by import By
from selenium.webdriver.support.ui import WebDriverWait
from selenium.webdriver.support import expected_conditions as EC
from selenium.webdriver.chrome.options import Options
from selenium.webdriver.common.keys import Keys
import re

# === Profile Setup ===
profile_path = r"C:\Users\yolol\Downloads\ScrapingProfile"  # CHANGE THIS
for fname in ["SingletonLock", "SingletonSocket", "SingletonCookie"]:
    try:
        os.remove(os.path.join(profile_path, fname))
    except FileNotFoundError:
        pass

chrome_options = Options()
chrome_options.add_argument(f"--user-data-dir={profile_path}")
chrome_options.add_argument("--profile-directory=Default")  # profile inside ScrapingProfile

driver = webdriver.Chrome(options=chrome_options)
wait = WebDriverWait(driver, 10)

# === Navigate to main page ===
base_url = "https://www.calorieking.com"
main_url = f"{base_url}/us/en/foods/b/calories-in-average-all-brands/cuNNkJVjRDOOxMUYAdBjMA"
driver.get(main_url)
time.sleep(2)

# === Accept Cookies ===
try:
    accept_btn = WebDriverWait(driver, 5).until(
        EC.element_to_be_clickable((By.CSS_SELECTOR, "a.cmpButtonLink.acceptLink"))
    )
    accept_btn.click()
    print("🍪 Accepted cookies.")
except:
    print("⚠️ Cookie banner not found or already accepted.")

# === Step 1: Collect Category Links ===
seen_links = set()

category_links = []
last_height = driver.execute_script("return document.body.scrollHeight")

while True:
    driver.execute_script("window.scrollTo(0, document.body.scrollHeight);")
    time.sleep(2)
    new_height = driver.execute_script("return document.body.scrollHeight")

    categories = driver.find_elements(By.CSS_SELECTOR, "h2.MuiTypography-root a")
    for cat in categories:
        name = cat.text.strip()
        link = cat.get_attribute("href")
        if (seen_links and link in seen_links) or not link:
            continue
        if link:
            seen_links.add(link)
            category_links.append({"name": name, "url": link})

    if new_height == last_height:
        break
    last_height = new_height

print(f"✅ Found {len(category_links)} categories.\n")

# === Step 2 & 3: Scrape each product from each category ===
all_products = []

for category_link in list(category_links):
    print(f"🔍 Visiting category: {category_link["name"]}")
    driver.get(category_link["url"])
    time.sleep(1)

    # Get product elements
    product_anchors = driver.find_elements(By.CSS_SELECTOR, "div.MuiList-root a.MuiButtonBase-root")
    products = []

    for anchor in product_anchors:
        href = anchor.get_attribute("href")
        name = anchor.text.strip()
        if href and "/us/en/foods/f/" in href:
            products.append({"url": href, "name": name})

    print(f"   → Found {len(products)} products.")

    for product in products:
        try:
            driver.get(product["url"])

            # Open unit dropdown associated with "Serving"
            try:
                unit_dropdown = wait.until(EC.presence_of_element_located((
                    By.XPATH,
                    "//p[text()='Serving']/preceding::div[@role='button' and contains(@class, 'MuiSelect-root')][1]"
                )))
                driver.execute_script("arguments[0].scrollIntoView(true);", unit_dropdown)
                if unit_dropdown.is_enabled():
                    unit_dropdown.click()
                    time.sleep(1)

                    unit_options = wait.until(EC.presence_of_all_elements_located((By.CSS_SELECTOR, "ul[role='listbox'] li")))
                    if unit_options:
                        last_option = unit_options[-1]
                        unit_text = last_option.text.strip()
                        last_option.click()
                    else:
                        unit_text = unit_dropdown.text.strip()
                else:
                    unit_text = unit_dropdown.text.strip()
            except Exception as e:
                print(f"   ⚠️ Unit dropdown not available or failed to interact: {e}")
                try:
                    unit_text = driver.find_element(By.XPATH, "//p[text()='Serving']/following-sibling::p").text.strip()
                except:
                    unit_text = ""

            # Set quantity to 100
            qty_input = wait.until(EC.presence_of_element_located((By.CSS_SELECTOR, "input[type='number']")))
            qty_input.click()
            qty_input.send_keys(Keys.BACKSPACE *5) 
            qty_input.send_keys("100")
            quantity = "100"
            time.sleep(0.1)

            # Read nutrition values
            wait.until(EC.presence_of_element_located((By.XPATH, "//div[contains(@class, 'MuiCollapse-wrapperInner')]//table")))
            nutrition_rows = driver.find_elements(By.XPATH, "//div[contains(@class, 'MuiCollapse-wrapperInner')]//table//tr")

            nutrition_data = {}

            for row in nutrition_rows:
                try:
                    label_cell = row.find_element(By.TAG_NAME, "th")
                    value_cells = row.find_elements(By.TAG_NAME, "td")

                    # Handle Calories row (value embedded in <th>)
                    calories_match = re.search(r"Calories\s*(\d+)", label_cell.text)
                    if calories_match:
                        nutrition_data["Calories"] = int(calories_match.group(1))
                        continue

                    label_text = label_cell.text.strip()
                    if not label_text or not value_cells:
                        continue

                    value_text = value_cells[0].text.strip()
                    value_text = value_text.replace(">", "> ").replace("<", "< ")

                    # Match e.g. "<1 mg", ">0.1mg", "3.4 g", etc.
                    value_match = re.search(r"([<>])?\s*([\d.]+)\s*([a-zA-Zμ%]+)", value_text)
                    if value_match:
                        symbol = value_match.group(1)  # "<" or ">" or None
                        number = value_match.group(2)  # e.g., "1", "0.1"
                        unit = value_match.group(3)    # e.g., "mg", "g", "%"

                        json_key = label_text.replace(" ", "")
                        nutrition_data[json_key] = {
                            "value": number,
                            "unit": unit,
                            "lessThan": symbol == "<",
                            "moreThan": symbol == ">"
                        }

                except Exception:
                    continue

            all_products.append({
                "name": product["name"],
                "unit": unit_text,
                "category": category_link["name"],
                "quantity": quantity,
                "nutrition": nutrition_data
            })

        except Exception as e:
            print(f"   ❌ Error on {product['url']}: {e}")
            continue
        
driver.quit()

# === Save to JSON ===
with open("calorieking_us_products.json", "w", encoding="utf-8") as f:
    json.dump(all_products, f, ensure_ascii=False, indent=2)
print("📦 Saved as calorieking_products.json")

# === Save to CSV ===
fieldnames = sorted(set().union(*[p.keys() for p in all_products]))

with open("calorieking_us_products.csv", "w", encoding="utf-8", newline='') as f:
    writer = csv.DictWriter(f, fieldnames=fieldnames)
    writer.writeheader()
    for product in all_products:
        writer.writerow(product)

print("📦 Saved as calorieking_products.csv")
