import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EditLearningTask } from './edit-learning-task';

describe('EditLearningTask', () => {
  let component: EditLearningTask;
  let fixture: ComponentFixture<EditLearningTask>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [EditLearningTask],
    }).compileComponents();

    fixture = TestBed.createComponent(EditLearningTask);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
