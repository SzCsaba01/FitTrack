import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ManageFoods } from './manage-foods';

describe('ManageFoods', () => {
  let component: ManageFoods;
  let fixture: ComponentFixture<ManageFoods>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ManageFoods]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ManageFoods);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
