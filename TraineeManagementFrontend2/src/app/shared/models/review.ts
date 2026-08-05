export interface Review {
  id: number;
  submissionId: number;
  mentorId: number;
  feedback: string;
  reviewStatus: string;
  reviewedDate: string;
  score: number;
  mentorName: string;
  submissionUrl: string;
}