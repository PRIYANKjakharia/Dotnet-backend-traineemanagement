import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AddLearningTask } from './add-learning-task';

describe('AddLearningTask', () => {
  let component: AddLearningTask;
  let fixture: ComponentFixture<AddLearningTask>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AddLearningTask],
    }).compileComponents();

    fixture = TestBed.createComponent(AddLearningTask);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
