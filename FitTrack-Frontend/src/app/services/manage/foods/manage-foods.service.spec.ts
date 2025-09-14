import { TestBed } from '@angular/core/testing';

import { ManageFoodsService } from './manage-foods.service';

describe('ManageFoodsService', () => {
  let service: ManageFoodsService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ManageFoodsService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
