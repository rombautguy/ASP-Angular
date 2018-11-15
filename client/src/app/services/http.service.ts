import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http'
import { AppSettings } from '../config/app-settings'
import 'rxjs/add/operator/map';

@Injectable()
export class HttpService {

  constructor(
    private httpClient: HttpClient,
  ) { }

  get(url, params = null): any {
    var apiUrl = AppSettings.API_ENDPOINT_FULL + url;
    return this.httpClient.get(apiUrl, { params })
  }
  post(url, params = {}): any {
    const options = {
      headers: new HttpHeaders({
        'Content-Type': 'application/x-www-form-urlencoded'
      })
    }
    var apiUrl = AppSettings.API_ENDPOINT_FULL + url;
    return this.httpClient.post(apiUrl, $.param(params), options)
  }
}

