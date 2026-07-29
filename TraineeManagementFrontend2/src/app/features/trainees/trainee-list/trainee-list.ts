import { Component, OnInit, signal } from '@angular/core';
import { TraineeService } from '../trainee.service';
import { TraineeQuery } from '../../../shared/models/trainee-query';
import { Trainee } from '../../../shared/models/trainee';
import { ChangeDetectorRef } from '@angular/core';
import { NgZone } from '@angular/core';
@Component({
  selector: 'app-trainee-list',
  imports: [],
  templateUrl: './trainee-list.html',
  styleUrl: './trainee-list.css'
})
export class TraineeList implements OnInit {

  constructor( private cdr : ChangeDetectorRef,private traineeService: TraineeService , private zone: NgZone) {}
  trainees: Trainee[] = [];
  loading = signal<boolean>(false);
  errorMessage = '';
  ngOnInit(): void {

    const query: TraineeQuery = {
      pageNumber: 1,
      pageSize: 10,
      search: '',
      status: ''
    };

    this.loading.set(true);

    this.traineeService.getAll(query).subscribe({
      next: (response) => {
        // this.zone.run(()=>{

          this.trainees = response.data;
          // console.log("loading before ",this.loading);
          this.loading.set(false);
          // console.log("loading after ",this.loading);
          // console.log("here");
          // this.cdr.detectChanges();
          
          // queueMicrotask(()=>{
            //   this.loading = false;
            // });
        // });
      },
      error: (error) => {

        if (error.status === 401) {
          this.errorMessage = 'Unauthorized';
        } else if (error.status === 403) {
          this.errorMessage = 'Forbidden';
        } else {
          this.errorMessage = 'Something went wrong.';
        }

        this.loading.set(false);
      }
    });
  }
}