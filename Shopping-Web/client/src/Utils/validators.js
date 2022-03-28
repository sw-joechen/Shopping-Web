// 帳號規則: 英文開頭, 英數皆可 限6~20字元
const accountRegex = new RegExp('^[A-Za-z][A-Za-z0-9]{5,19}$');

export const IsAccountValid = (account) => {
  return accountRegex.test(account);
};

// 密碼規則: 6 位數以上，並且至少包含大寫字母、小寫字母、數字各一
const pwdRegex = new RegExp(
  '^(?=.*[0-9])(?=.*[A-Z])(?=.*[a-z])[a-zA-Z0-9!@#$%^&*]{6,}$'
);

export const IsPwdValid = (pwd) => {
  return pwdRegex.test(pwd);
};

// 過濾特殊字元(包含空格)
const specialCharactersRegex = new RegExp(
  /[\s`~!@#$%^&*()_+\-=[\]{};':"\\|,.<>/?]+/
);

export const IsContaineSpecialCharaters = (str) => {
  return specialCharactersRegex.test(str);
};

// 純數字
const numberRegex = new RegExp(/^\d+$/);

export const IsPureNumber = (number) => {
  return numberRegex.test(number);
};
