import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AddTrainee } from './add-trainee';

describe('AddTrainee', () => {
  let component: AddTrainee;
  let fixture: ComponentFixture<AddTrainee>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AddTrainee],
    }).compileComponents();

    fixture = TestBed.createComponent(AddTrainee);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
