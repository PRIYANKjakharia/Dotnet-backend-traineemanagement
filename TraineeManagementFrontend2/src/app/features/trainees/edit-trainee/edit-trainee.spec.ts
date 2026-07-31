import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EditTrainee } from './edit-trainee';

describe('EditTrainee', () => {
  let component: EditTrainee;
  let fixture: ComponentFixture<EditTrainee>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [EditTrainee],
    }).compileComponents();

    fixture = TestBed.createComponent(EditTrainee);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
