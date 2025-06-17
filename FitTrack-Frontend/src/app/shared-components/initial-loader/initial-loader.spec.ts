import { ComponentFixture, TestBed } from '@angular/core/testing';

import { InitialLoader } from './initial-loader';

describe('InitialLoader', () => {
  let component: InitialLoader;
  let fixture: ComponentFixture<InitialLoader>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [InitialLoader]
    })
    .compileComponents();

    fixture = TestBed.createComponent(InitialLoader);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
