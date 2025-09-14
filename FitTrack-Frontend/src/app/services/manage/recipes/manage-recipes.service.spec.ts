import { TestBed } from '@angular/core/testing';

import { ManageRecipesService } from './manage-recipes.service';

describe('ManageRecipesService', () => {
  let service: ManageRecipesService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ManageRecipesService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
