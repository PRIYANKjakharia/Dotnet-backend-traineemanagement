export interface TaskAssignment {
  id: number;
  traineeId: number;
  traineeName: string;
  mentorId: number;
  mentorName: string;
  learningTaskId: number;
  learningTaskTitle: string;
  assignedDate: Date;
  dueDate: Date;
  status: string;
  remarks: string;
}