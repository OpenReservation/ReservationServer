export function formatTime(date: Date): string {
  const year = date.getFullYear()
  const month = date.getMonth() + 1
  const day = date.getDate()
  const hour = date.getHours()
  const minute = date.getMinutes()
  const second = date.getSeconds()

  return [year, month, day].map(formatNumber).join('/') + ' ' + [hour, minute, second].map(formatNumber).join(':');
}

export function addDays(date: Date, days: number) {
  var adjustDate = new Date(date.getTime() + 24 * 60 * 60 * 1000 * days);
  return adjustDate;
}

export function formatDate(date: Date): string {
  var year = date.getFullYear()
  var month = date.getMonth() + 1
  var day = date.getDate()

  return [year, month, day].map(formatNumber).join('-');
}

const formatNumber = (n: number) => {
  const str = n.toString()
  return str[1] ? str : '0' + str;
}
