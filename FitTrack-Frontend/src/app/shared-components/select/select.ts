import {
  ChangeDetectionStrategy,
  Component,
  computed,
  ElementRef,
  HostListener,
  input,
  signal,
} from '@angular/core';
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';
import { SelectOption } from '../../models/select-option.model';

@Component({
  selector: 'app-select',
  standalone: true,
  imports: [],
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: Select,
      multi: true,
    },
  ],
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './select.html',
  styleUrl: './select.scss',
})
export class Select implements ControlValueAccessor {
  options = input.required<SelectOption[]>();
  placeholder = input<string>('Select...');
  multiple = input<boolean>(false);
  selectedOption = signal<SelectOption | null>(null);
  selectedOptions = signal<SelectOption[] | null>(null);
  isDisabled = signal<boolean>(false);
  isDropdownOpen = signal<boolean>(false);
  private onChange: (_: any) => void = () => {};
  private onTouched: () => void = () => {};

  selectedOptionsString = computed(() => {
    if (!this.selectedOptions()) {
      return '';
    }
    return this.selectedOptions()!
      .map((option) => option.label)
      .join(', ');
  });

  constructor(private elementRef: ElementRef<HTMLElement>) {}

  @HostListener('document:click', ['$event'])
  onDocumentClick(event: MouseEvent) {
    const clickedInside = this.elementRef.nativeElement.contains(
      event.target as Node,
    );
    if (!clickedInside && this.isDropdownOpen()) {
      this.onCloseDropdown();
    }
  }

  writeValue(value: any): void {
    if (!this.options() || this.options().length === 0) {
      this.selectedOption.set(null);
      this.selectedOptions.set([]);
      return;
    }

    if (this.multiple()) {
      if (Array.isArray(value)) {
        const selectedOptions = this.options().filter((o) =>
          value.includes(o.value),
        );
        this.selectedOptions.set(selectedOptions);
      } else {
        this.selectedOptions.set([]);
      }
    } else {
      const selectedOption =
        this.options().find((o) => o.value === value) ?? null;
      this.selectedOption.set(selectedOption);
    }
  }

  registerOnChange(fn: any): void {
    this.onChange = fn;
  }

  registerOnTouched(fn: any): void {
    this.onTouched = fn;
  }

  setDisabledState?(isDisabled: boolean): void {
    this.isDisabled.set(isDisabled);
  }

  onToggleDropdown() {
    if (this.isDisabled()) {
      return;
    }

    this.isDropdownOpen.update((x) => !x);
    this.onTouched();
  }

  private onCloseDropdown() {
    this.isDropdownOpen.set(false);
  }

  onSingleOptionClick(option: SelectOption) {
    if (this.isDisabled()) {
      return;
    }

    this.selectedOption.set(option);
    this.onChange(option.value);
    this.onCloseDropdown();
    this.onTouched();
  }

  onMultiOptionClick(option: SelectOption) {
    if (this.isDisabled()) {
      return;
    }

    this.selectedOptions.update((options) => {
      if (!options) {
        return options;
      }
      const currentLength = options?.length;

      const newOptions = options?.filter((o) => o.value !== option.value);
      if (newOptions?.length != currentLength) {
        return newOptions;
      }

      return [...options, option];
    });

    this.onChange(this.selectedOptions()!.map((x) => x.value));
    this.onTouched();
  }

  isSelected(option: SelectOption): boolean {
    return (
      this.selectedOptions()?.some((o) => o.value === option.value) ?? false
    );
  }
}
