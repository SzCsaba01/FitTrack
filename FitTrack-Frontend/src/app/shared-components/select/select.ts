import {
  ChangeDetectionStrategy,
  Component,
  computed,
  ElementRef,
  HostListener,
  input,
  Signal,
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
  selectedOptionSignal = signal<SelectOption | null>(null);
  selectedOptionsSignal = signal<SelectOption[] | null>(null);
  isDisabledSignal = signal<boolean>(false);
  isDropdownOpenSignal = signal<boolean>(false);
  private onChange: (_: any) => void = () => {};
  private onTouched: () => void = () => {};

  selectedOptionsString: Signal<string> = computed(() => {
    if (!this.selectedOptionsSignal()) {
      return '';
    }
    return this.selectedOptionsSignal()!
      .map((option) => option.label)
      .join(', ');
  });

  constructor(private elementRef: ElementRef<HTMLElement>) {}

  @HostListener('document:click', ['$event'])
  onDocumentClick(event: MouseEvent) {
    const clickedInside = this.elementRef.nativeElement.contains(
      event.target as Node,
    );
    if (!clickedInside && this.isDropdownOpenSignal()) {
      this.onCloseDropdown();
    }
  }

  writeValue(value: any): void {
    if (this.multiple()) {
      this.selectedOptionsSignal.set(Array.isArray(value) ? value : []);
    } else {
      this.selectedOptionSignal.set(value);
    }
  }

  registerOnChange(fn: any): void {
    this.onChange = fn;
  }

  registerOnTouched(fn: any): void {
    this.onTouched = fn;
  }

  setDisabledState?(isDisabled: boolean): void {
    this.isDisabledSignal.set(isDisabled);
  }

  onToggleDropdown() {
    if (this.isDisabledSignal()) {
      return;
    }

    this.isDropdownOpenSignal.update((x) => !x);
    this.onTouched();
  }

  private onCloseDropdown() {
    this.isDropdownOpenSignal.set(false);
  }

  onSingleOptionClick(option: SelectOption) {
    if (this.isDisabledSignal()) {
      return;
    }

    this.selectedOptionSignal.set(option);
    this.onChange(option.value);
    this.onCloseDropdown();
    this.onTouched();
  }

  onMultiOptionClick(option: SelectOption) {
    if (this.isDisabledSignal()) {
      return;
    }

    this.selectedOptionsSignal.update((options) => {
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

    this.onChange(this.selectedOptionsSignal()!.map((x) => x.value));
    this.onTouched();
  }

  isSelected(option: SelectOption): boolean {
    return (
      this.selectedOptionsSignal()?.some((o) => o.value === option.value) ??
      false
    );
  }
}
