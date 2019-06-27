declare class TencentCaptcha {
    constructor(captchaDom: any, appId: string, callback: (res:any)=>void);
    destroy():void;
    show():void;
}