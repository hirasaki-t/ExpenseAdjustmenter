export type Classification = "諸経費" | "旅費・交通費";
export type SubmissionMethod = "電子" | "紙/持参" | "紙/郵送";
export type ApproveStatus = "申請中" | "否認" | "承認";
export type WorkName = "社内業務" | "営業" | "現場業務";

export type TraveringExpense = {
  id: string;
  date: Date;
  workName: WorkName;
  startSection: string | null;
  endSection: string | null;
  categoryId: string;
  submissionMethod: SubmissionMethod | null;
  receipt: File | null;
  amount: number;
  remarks: string | null;
};

export type SundryExpense = {
  id: string;
  date: Date;
  expenseTypeId: string;
  submissionMethod: SubmissionMethod | null;
  details: string | null;
  participationNumber: number;
  receipt: File | null;
  amount: number;
  status: ApproveStatus | null;
};

export type ApproveHistory = {
  id: string;
  expenseId: string;
  date: Date;
  userId: string;
  type: string;
  amount: number;
  submissionMethod: SubmissionMethod | null;
  receiptId: string | null;
  reviewerId: string | null;
  comment: string | null;
  status: string | null;
  workName: string | null;
  startSection: string | null;
  endSection: string | null;
  categoryId: string | null;
  remarks: string | null;
  expenseTypeId: string | null;
  participationNumber: number | null;
  details: string | null;
};

export type ApproveHistoryData = {
  date: Date;
  userId: string | null;
  comment: string | null;
  status: string;
  color: string;
};

export type TrafficCategory = {
  id: string;
  name: string;
  details: string | null;
  isReceipt: boolean;
  isActive: boolean;
};

export type ExpenseType = {
  id: string;
  name: string;
  details: string | null;
  isReceipt: boolean;
  isActive: boolean;
};

export type GraphUser = {
  displayName: string;
  mail: string;
};

export type ManagementListData = {
  date: Date;
  userName: string;
  type: string;
  amount: number;
  receipt: string | null;
  userId: string | null;
  comment: string | null;
  status: string | null;
};

export type Me = {
  displayName: string;
  mail: string;
};

export type SundryExpenseListData = {
  id: string;
  date: Date;
  expenseTypeId: string;
  details: string | null;
  participationNumber: number;
  receiptId: string | null;
  submissionMethod: SubmissionMethod | null;
  amount: number;
  reviewerId: string | null;
  comment: string | null;
  status: ApproveStatus | null;
};

export type TraveringExpenseTemplate = {
  name: string;
  workName: string;
  startSection: string;
  endSection: string;
  isGoBack: boolean;
  category: string;
  amount: number;
};

export type TraveringExpenseListData = {
  id: string;
  date: Date;
  startSection: string | null;
  endSection: string | null;
  categoryId: string;
  workName: WorkName;
  remarks: string | null;
  amount: number;
  receiptId: string | null;
  submissionMethod: SubmissionMethod | null;
  reviewerId: string | null;
  comment: string | null;
  status: ApproveStatus | null;
};

export type User = {
  id: string;
  name: string;
  mail: string;
  isAdmin: boolean;
  isActive: boolean;
};

export interface AspErrorData {
  detail: string;
}

export interface IdObject {
  id: string;
}
