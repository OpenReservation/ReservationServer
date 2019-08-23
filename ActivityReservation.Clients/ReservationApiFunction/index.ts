import { HttpRequester } from './httpRequester';

const apiBaseUrl = "https://reservation.weihanli.xyz";
const functionName = "reservationWxAppGateway";

exports.main_handler = async function (event: any, context: any, callback: any): Promise<object> {

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
    console.log(`request url: ${url}, method: ${event.httpMethod}`);
    let response = await HttpRequester.GetResponse(url, event.httpMethod, event.headers, event.body);

    return {
        isBase64: false,
        statusCode: response.statusCode,
        headers: response.responseHeaders,
        body: response.responseText
    };
}

// (async () => {
//   var event = new Event();
//   event.path = "/api/notice";
//   event.headers = {
//       "X-A": "AAA"
//   };
//   event.httpMethod = "GET";
//   event.queryString = {
//       pageNumber: 1
//   };

//   var response = await main(event,null, null);
//   console.log(response);
// })();
