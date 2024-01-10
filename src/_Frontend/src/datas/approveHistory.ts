import { SubmissionMethod } from "../types";

export type ApproveHistory = {
    id: string,
    expenseId: string,
    date: Date,
    userId: string,
    type: string,
    amount: number,
    submissionMethod: SubmissionMethod | null,
    receiptId: string | null,
    reviewerId: string | null,
    comment: string | null,
    status: string | null,
    workName: string | null,
    startSection: string | null,
    endSection: string | null,
    categoryId: string | null,
    remarks: string | null,
    expenseTypeId: string | null,
    participationNumber: number | null,
    details: string | null
}