import type { FormRules } from "element-plus";

export const traveringExpenseRules: FormRules = {
    date: {
        required: true,
        message: "必須項目です",
        trigger: "blur",
    },
    workName: {
        required: true,
        message: "必須項目です",
        trigger: "blur",
    },
    category: {
        required: true,
        message: "必須項目です",
        trigger: "blur",
    },
    amount: {
        required: true,
        pattern: /[0-9]/,
        message: "半角数値で必須項目です",
        trigger: "blur",
    },
    submissionMethod: {
        required: true,
        message: "必須項目です",
        trigger: "blur",
    },
    receipt: {
        required: true,
        message: "必須項目です",
        trigger: "blur",
    },
};