import http from 'k6/http';
import { check } from 'k6';

// export let options = {
//   stages: [
//     { duration: '30s', target: 20 },
//     { duration: '1m30s', target: 10 },
//     { duration: '20s', target: 0 },
//   ]
// };

export let options = {
  vus: 50,
  duration: '5m',
};

const host = "https://reservation.weihanli.xyz";

export default function () {
  let res = http.get(`${host}/api/notice`);
  check(res, { 'status was 200': (r) => r.status == 200 });
}
