import requests
from bs4 import BeautifulSoup
import pandas as pd
import json
import time
import re

BASE_URL = "https://www.allrecipes.com"
START_URL = "https://www.allrecipes.com/recipes/84/healthy-recipes/"
HEADERS = {"User-Agent": "Mozilla/5.0"}

def get_subcategories(url):
    res = requests.get(url, headers=HEADERS)
    soup = BeautifulSoup(res.text, "html.parser")
    taxonomy_links = soup.select("li.mntl-taxonomy-nodes__item a")

    categories = []
    for link in taxonomy_links:
        name = link.get_text(strip=True)
        href = link["href"]
        if href.startswith("https://www.allrecipes.com/recipes/"):
            categories.append({"name": name, "url": href})
    return categories

def parse_recipe_details(url):
    try:
        res = requests.get(url, headers=HEADERS)
        soup = BeautifulSoup(res.text, "html.parser")

        details = {}

        # Times and servings
        time_items = soup.select(".mm-recipes-details__item")
        for item in time_items:
            label = item.select_one(".mm-recipes-details__label").get_text(strip=True).rstrip(":")
            value = item.select_one(".mm-recipes-details__value").get_text(strip=True)
            details[label.lower().replace(" ", "_")] = value

        # Ingredients
        ingredients = []
        ingredient_items = soup.select(".mm-recipes-structured-ingredients__list-item")

        for item in ingredient_items:
            qty_el = item.select_one('[data-ingredient-quantity="true"]')
            unit_el = item.select_one('[data-ingredient-unit="true"]')
            name_el = item.select_one('[data-ingredient-name="true"]')

            quantity = qty_el.get_text(strip=True) if qty_el else ""
            unit = unit_el.get_text(strip=True) if unit_el else ""
            name = name_el.get_text(strip=True) if name_el else ""

            ingredients.append({
                "quantity": quantity,
                "unit": unit,
                "name": name
            })

        details["ingredients"] = ingredients


        # Directions
        directions = []
        direction_items = soup.select("#mm-recipes-steps__content_1-0 ol li p")
        for item in direction_items:
            step = item.get_text(strip=True)
            directions.append(step)
        details["directions"] = directions

        # Nutrition Facts
        nutrition = {}

        # Calories
        cal_el = soup.select_one(".mm-recipes-nutrition-facts-label__calories span:nth-of-type(2)")
        if cal_el:
            nutrition["calories"] = {"value": int(re.sub(r"[^\d]", "", cal_el.text)), "unit": ""}

        wanted = {
            "total fat": "total_fat",
            "saturated fat": "saturated_fat",
            "cholesterol": "cholesterol",
            "sodium": "sodium",
            "total carbohydrate": "total_carbohydrate",
            "dietary fiber": "dietary_fiber",
            "total sugars": "total_sugars",
            "protein": "protein",
            "vitamin c": "vitamin_c",
            "calcium": "calcium",
            "iron": "iron",
            "potassium": "potassium"
        }

        rows = soup.select("tbody.mm-recipes-nutrition-facts-label__table-body tr")

        for row in rows:
            cols = row.find_all("td")
            if len(cols) == 2:
                name_el = cols[0].find("span")
                if not name_el:
                    continue
                label = name_el.text.strip().lower()
                if label in wanted:
                    raw = cols[0].get_text(strip=True).replace(name_el.text.strip(), "").strip()
                    value_match = re.match(r"([\d.]+)([a-zA-Z]*)", raw)
                    if value_match:
                        val, unit = value_match.groups()
                        nutrition[wanted[label]] = {
                            "value": float(val),
                            "unit": unit
                        }

            elif len(cols) == 1 and "Total Sugars" in row.text:
                name_el = row.find("span")
                if name_el:
                    label = name_el.text.strip().lower()
                    if label in wanted:
                        raw = row.get_text(strip=True).replace(name_el.text.strip(), "").strip()
                        value_match = re.match(r"([\d.]+)([a-zA-Z]*)", raw)
                        if value_match:
                            val, unit = value_match.groups()
                            nutrition[wanted[label]] = {
                                "value": float(val),
                                "unit": unit
                            }

        details["nutrition"] = nutrition
        return details

    except Exception as e:
        print(f"❌ Error parsing recipe details from {url}: {e}")
        return {}

def get_recipes_from_category(cat_name, cat_url):
    print(f"📦 Scraping category: {cat_name} ({cat_url})")
    res = requests.get(cat_url, headers=HEADERS)
    soup = BeautifulSoup(res.text, "html.parser")
    recipe_cards = soup.select("a.mntl-card-list-items")

    recipes = []
    for card in recipe_cards:
        try:
            title_el = card.select_one(".card__title-text")
            title = title_el.get_text(strip=True) if title_el else "No Title"
            url = card["href"]

            rating_el = card.select(".icon-star")
            rating = len(rating_el)  # each star is an svg

            ratings_count_el = card.select_one(".mntl-recipe-card-meta__rating-count-number")
            ratings_count = ratings_count_el.get_text(strip=True).split()[0] if ratings_count_el else "0"

            # Go to recipe page and extract details
            details = parse_recipe_details(url)

            recipes.append({
                "title": title,
                "category": cat_name,
                **details  # merge details into main dict
            })

            time.sleep(0.5)  # Slight pause between recipe detail requests
        except Exception as e:
            print(f"❌ Error parsing card: {e}")
            continue
    return recipes

def main():
    all_data = []
    subcategories = get_subcategories(START_URL)
    print(f"🔍 Found {len(subcategories)} subcategories")

    for category in subcategories:
        cat_name = category["name"]
        cat_url = category["url"]
        recipes = get_recipes_from_category(cat_name, cat_url)
        all_data.extend(recipes)
        time.sleep(1)

    # Save as CSV and JSON
    df = pd.DataFrame(all_data)
    df.to_csv("healthy_recipes_detailed.csv", index=False)

    with open("healthy_recipes_detailed.json", "w", encoding="utf-8") as f:
        json.dump(all_data, f, ensure_ascii=False, indent=2)

    print("✅ Done. Data saved to healthy_recipes_detailed.csv and healthy_recipes_detailed.json")

if __name__ == "__main__":
    main()
