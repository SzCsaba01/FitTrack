import { TestBed } from '@angular/core/testing';

import { ManageExercisesService } from './manage-exercises.service';

describe('ManageExercisesService', () => {
  let service: ManageExercisesService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ManageExercisesService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
