import { IncomingHttpHeaders, OutgoingHttpHeaders } from "http";
import * as got from 'got';

export class HttpRequester{    
    public static async GetResponse(url:string, method:string, headers:OutgoingHttpHeaders, body?:string): Promise<HttpResponse> {        
        headers["host"]= undefined;
        console.log(`request info, url: ${url}, method: ${method}, headers: ${JSON.stringify(headers)}`);

        let result = await got.default(url,{
            body: body,
            method: method,
            headers: headers
        });
        var response = new HttpResponse();
        response.statusCode = result.statusCode;
        response.responseText = result.body;
        response.responseHeaders = result.headers;

        return response;
    }
}

export class HttpResponse{
    
    private _statusCode : number = -1;
    public get statusCode() : number {
        return this._statusCode;
    }
    public set statusCode(v : number) {
        this._statusCode = v;
    }
    
    
    private _responseText : string = "";
    public get responseText() : string {
        return this._responseText;
    }
    public set responseText(v : string) {
        this._responseText = v;
    }
    
    private _headers : IncomingHttpHeaders|undefined = undefined;
    public get responseHeaders() : IncomingHttpHeaders|undefined {
        return this._headers;
    }
    public set responseHeaders(v : IncomingHttpHeaders|undefined) {
        this._headers = v;
    }
    
}