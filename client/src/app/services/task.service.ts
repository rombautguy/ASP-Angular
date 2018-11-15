import { Injectable } from '@angular/core';
import { HttpService } from './http.service';
import { Task } from '../model/task.model';

@Injectable()
export class TaskService {
  constructor(private httpService: HttpService) { }

  getTasks(userid: number) {
    return this.httpService.get(`Task/getTasks?userid=${userid}`)
  }
  createTask(task: Task) {
    return this.httpService.post('Task/createTask', task)
  }
  updateTask(task: Task) {
    return this.httpService.post('Task/updateTask', task)
  }
  deleteUser(id: number) {
    return this.httpService.get(`Task/deleteUser?id=${id}`)
  }
}
