import { Component, OnInit, signal } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router } from '@angular/router';

import { TaskAssignmentService } from '../task-assignment.service';
import { TraineeService } from '../../trainees/trainee.service';
import { MentorService } from '../../mentors/mentor.service';
import { LearningTaskService } from '../../learning-tasks/learning-task.service';

import { Trainee } from '../../../shared/models/trainee';
import { Mentor } from '../../../shared/models/mentor';
import { LearningTask } from '../../../shared/models/learning-task';

@Component({
  selector: 'app-add-task-assignment',
  imports: [ReactiveFormsModule],
  templateUrl: './add-task-assignment.html',
  styleUrl: './add-task-assignment.css'
})
export class AddTaskAssignment implements OnInit {

  constructor(
    private fb: FormBuilder,
    private taskAssignmentService: TaskAssignmentService,
    private traineeService: TraineeService,
    private mentorService: MentorService,
    private learningTaskService: LearningTaskService,
    private router: Router
  ) {}

  taskAssignmentForm!: FormGroup;

  trainees = signal<Trainee[]>([]);
  mentors = signal<Mentor[]>([]);
  learningTasks = signal<LearningTask[]>([]);

  loading = signal(false);
  errorMessage = signal('');
  successMessage = signal('');
  dropdownsLoaded = signal(false);

  ngOnInit(): void {

    this.taskAssignmentForm = this.fb.group({

      traineeId: ['', Validators.required],
      mentorId: ['', Validators.required],
      learningTaskId: ['', Validators.required],
      status: ['', Validators.required],
      remarks: ['']

    });

    this.loadDropdowns();
    this.dropdownsLoaded.set(true);

  }

  loadDropdowns(): void {

    this.traineeService.getAll({
      pageNumber: 1,
      pageSize: 1000,
      search: '',
      status: ''
    }).subscribe(res => {
      this.trainees.set(res.data);
      console.log("Trainees:" , res.data);
      // this.trainees =[...res.data];
    });

    this.mentorService.getAll().subscribe(res => this.mentors.set(res));

    this.learningTaskService.getAll().subscribe(res => this.learningTasks.set(res));
    // this.dropdownsLoaded.set(true);

  }

  onSubmit(): void {

    this.errorMessage.set('');
    this.successMessage.set('');

    if (this.taskAssignmentForm.invalid) {

      this.taskAssignmentForm.markAllAsTouched();
      return;

    }

    this.loading.set(true);

    this.taskAssignmentService.create(this.taskAssignmentForm.value).subscribe({

      next: () => {

        this.successMessage.set('Task Assignment created successfully.');

        setTimeout(() => {

          this.router.navigate(['/taskassignments']);

        },1000);

      },

      error: (error) => {

        if(error.status === 400){

          this.errorMessage.set(error.error.message ?? 'Validation failed');

        }else{

          this.errorMessage.set('Something went wrong.');

        }

        this.loading.set(false);

      }

    });

  }

  cancel(): void {

    this.router.navigate(['/taskassignments']);

  }

}