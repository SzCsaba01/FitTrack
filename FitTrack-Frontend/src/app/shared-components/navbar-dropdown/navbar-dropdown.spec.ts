import { ComponentFixture, TestBed } from '@angular/core/testing';

import { NavbarDropdown } from './navbar-dropdown';

describe('NavbarDropdown', () => {
  let component: NavbarDropdown;
  let fixture: ComponentFixture<NavbarDropdown>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [NavbarDropdown]
    })
    .compileComponents();

    fixture = TestBed.createComponent(NavbarDropdown);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
