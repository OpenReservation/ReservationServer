import { HttpRequester } from './httpRequester';

const apiBaseUrl = "https://reservation.weihanli.xyz";
const functionName = "reservationWxAppGateway";

async function main (event: any, context: any, callback: any): Promise<object> {
    console.log(event);

    let queryString = "";
    if (event.queryString) {
        queryString = "?";
        let i = 0;
        for (var key in event.queryString) {
            if (i > 0) {
                queryString += `&`;
            } else {
                i++;
            }
            queryString += `${key}=${event.queryString[key]}`;
        }
    }

    if((<string>event.path).startsWith(`/${functionName}`)){
        event.path = (<string>event.path).replace(`/${functionName}`, '');
    }

    let url = `${apiBaseUrl}${event.path}${queryString}`;
    console.log(`request url: ${url}, method: ${event.httpMethod}, headers:${JSON.stringify(event.headers)}`);
    let response = await HttpRequester.GetResponse(url, event.httpMethod, event.headers, event.body);
    console.log(`response: ${JSON.stringify(response)}`);
    
    return {
        isBase64: false,
        statusCode: response.statusCode,
        headers: response.responseHeaders,
        body: response.responseText
    };
};

exports.main_handler = main;

// (async () => {
//   process.env.NODE_TLS_REJECT_UNAUTHORIZED="0";
//   var event:any = new Object();
//   event.path = "/api/reservation";
//   event.headers = {
//     "Content-Type": "application/json;charset=utf-8",
//     "X-Query": "abccd",
//     "User-Agent": "Chrome 79"
//   };
//   event.httpMethod = "GET";
//   event.body = '{"ReservationPlaceId":"6bbbcfa9-7b38-444d-8a80-5da9d06c62a7","ReservationPlaceName":"多媒体工作室","ReservationForDate":"2019-08-27","ReservationForTimeIds":"0","ReservationForTime":"08:00-10:00","ReservationUnit":"测试","ReservationActivityContent":"测测更健康","ReservationPersonName":"测试001","ReservationPersonPhone":"13222223333"}';
//   event.queryString = {
//       pageNumber: 1
//   };
//   var response = await main(event, null, null);
//   console.log(response);
// })();
