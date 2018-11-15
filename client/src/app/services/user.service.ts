import { Injectable } from '@angular/core';
import { HttpService } from './http.service';
import { User } from '../model/user.model';

@Injectable()
export class UserService {
  constructor(private httpService: HttpService) { }

  getUsers() {
    return this.httpService.get('User/GetUsers')
  }
  createUser(user: User) {
    return this.httpService.post('User/createUser', user)
  }
  updateUser(user: User) {
    return this.httpService.post('User/updateUser', user)
  }
  deleteUser(id: number) {
    return this.httpService.get(`User/deleteUser?id=${id}`)
  }
}
