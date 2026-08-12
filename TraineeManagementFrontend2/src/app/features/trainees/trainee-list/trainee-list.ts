import { Component, OnInit, signal } from '@angular/core';
import { TraineeService } from '../trainee.service';
import { TraineeQuery } from '../../../shared/models/trainee-query';
import { Trainee } from '../../../shared/models/trainee';
import { FormsModule } from '@angular/forms';
import { RouterLink } from "@angular/router";
import { AuthService } from '../../../core/services/auth.service';
@Component({
  selector: 'app-trainee-list',
  imports: [FormsModule, RouterLink],
  templateUrl: './trainee-list.html',
  styleUrl: './trainee-list.css'
})
export class TraineeList implements OnInit {
  
  constructor(private traineeService: TraineeService , public authService: AuthService) {}
  trainees: Trainee[] = [];
  loading = signal<boolean>(false);
  errorMessage = '';
  query: TraineeQuery = {
    pageNumber: 1,
    pageSize: 10,
    search: '',
    status: ''
  };
  totalRecords = 0;
  ngOnInit(): void {
    // this.loading.set(true);
    this.loadTrainees();
    // this.traineeService.getAll(this.query).subscribe({
      //   next: (response) => {
        // this.zone.run(()=>{
          
        // this.trainees = response.data;
        // console.log("loading before ",this.loading);
        // this.loading.set(false);
        // console.log("loading after ",this.loading);
        // console.log("here");
        // this.cdr.detectChanges();
        
        // queueMicrotask(()=>{
          //   this.loading = false;
        // });
      // });
    // },
    // error: (error) => {
            
    //   if (error.status === 401) {
    //     this.errorMessage = 'Unauthorized';
    //   } else if (error.status === 403) {
      //     this.errorMessage = 'Forbidden';
    //   } else {
      //     this.errorMessage = 'Something went wrong.';
    //   }
      
      //   this.loading.set(false);
      // }
    // });
  }
    
  search():void{
    this.query.pageNumber = 1;
    this.loadTrainees();
  }
  filterByStatus():void{
    this.query.pageNumber = 1;
    this.loadTrainees();
  }
  
  changePageSize() {
    this.query.pageNumber = 1;
    this.loadTrainees();
  }

  loadTrainees(): void {
    this.loading.set(true);
    this.trainees = [];
    this.errorMessage = '';
    this.traineeService.getAll(this.query).subscribe({
      next: (response) => {
        this.trainees = response.data;
        this.totalRecords = response.totalRecords;
        this.loading.set(false);
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

  previousPage(): void{
    if(this.query.pageNumber > 1){
      this.query.pageNumber--;
      this.loadTrainees();
    }
  }
  nextPage(): void{
    if(this.query.pageNumber * this.query.pageSize < this.totalRecords){
      this.query.pageNumber++;
      this.loadTrainees();
    }
  }
  // deleteTrainee(id: number): void {
  //   if (!confirm('Are you sure you want to delete this trainee?')) {
  //     return;
  //   }

  //   this.loading.set(true);
  //   this.errorMessage = '';

  //   this.traineeService.delete(id).subscribe({

  //     next: () => {
  //       this.loadTrainees();
  //     },

  //     error: (error) => {
  //       if(error.status === 400){
  //         this.errorMessage = error.error.message ?? 'Delete Failed';
  //       } else if (error.status === 404) {
  //         this.errorMessage = error.error.message ?? 'Trainee not found';
  //       } else {
  //         this.errorMessage = 'Something went wrong.';
  //       }

  //       this.loading.set(false);
  //     }

  //   });

  // }
}