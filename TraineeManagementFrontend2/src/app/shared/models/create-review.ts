export interface CreateReview {
  submissionId: number;
  mentorId: number;
  feedback: string;
  reviewStatus: string;
  score: number;
  reviewedDate: string;
}