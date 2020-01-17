import { environment } from "../../environments/environment";
import { Injectable } from '@angular/core';

@Injectable({
    providedIn: 'root'
})
export class ConfigService {

    public GetApiBaseUrl(): string {
        if (window["__env"] && window["__env"]["ApiBaseUrl"]) {           
            return <string>window["__env"]["ApiBaseUrl"];
        }
        return environment.apiBaseUrl;
    }
}