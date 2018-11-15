import { Component, OnInit, ViewChild, AfterViewInit } from '@angular/core';
import { LocalDataSource } from 'ng2-smart-table';
import {Ng2SmartTableComponent} from 'ng2-smart-table/ng2-smart-table.component';
import { UserService } from '../../services/user.service';
import { User } from '../../model/user.model';
import { Task } from '../../model/task.model';
import { TaskService } from '../../services/task.service';

const PriorityList = [
  {value: 1, title:'Critical'},
  {value: 2, title:'Medium'},
  {value: 3, title:'Low'}
]
const StateList = [
  {value: 1, title:'New'},
  {value: 2, title:'Active'},
  {value: 3, title:'Resolved'},
  {value: 4, title:'Closed'}
]

@Component({
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements AfterViewInit {
  users: LocalDataSource;
  tasks: LocalDataSource;

  selectedUser: User
  selectedTask: Task

  userTableSettings = {
    columns: {
      // id: {
      //   title: 'ID',
      //   filter: false,
      //   editable: false
      // },
      name: {
        title: 'User Name',
        filter: false
      }
    },
    add: {
      confirmCreate: true,
    },
    edit: {
      confirmSave: true,
      editButtonContent: '<i class="ti-pencil text-info m-r-10"></i>',
      saveButtonContent: '<i class="ti-save text-success m-r-10"></i>',
      cancelButtonContent: '<i class="ti-close text-danger"></i>',
    },
    delete: {
      confirmDelete: true,
      deleteButtonContent: '<i class="ti-trash text-danger m-r-10"></i>',
      saveButtonContent: '<i class="ti-save text-success m-r-10"></i>',
      cancelButtonContent: '<i class="ti-close text-danger"></i>',
    },
  }

  taskTableSettings = {
    columns: {
      // id: {
      //   title: 'ID',
      //   filter: false,
      //   editable: false
      // },
      title: {
        filter: false,
        title: 'Title'
      },
      description: {
        filter: false,
        title: 'Description'
      },
      priority: {
        filter: false,
        title: 'Priority Level',
        editor: {
          type: 'list',
          config: {
            selectText: 'Select',
            type: 'number',
            list: PriorityList,
          },
        },
        valuePrepareFunction: (cell, row) => { return PriorityList.filter(state => state.value == row.priority)[0].title }
      },
      state: {
        filter: false,
        title: 'State',
        editor: {
          type: 'list',
          config: {
            selectText: 'Select',
            type: 'number',
            list: StateList,
          },
        },
        valuePrepareFunction: (cell, row) => { return StateList.filter(state => state.value == row.state)[0].title }
      },
      estimate: {
        filter: false,
        title: 'Estimate',
        type: 'number',
        editor: {
          type: 'number'
        }
      }
    },
    add: {
      confirmCreate: true,
    },
    edit: {
      confirmSave: true,
      editButtonContent: '<i class="ti-pencil text-info m-r-10"></i>',
      saveButtonContent: '<i class="ti-save text-success m-r-10"></i>',
      cancelButtonContent: '<i class="ti-close text-danger"></i>',
    },
    delete: {
      confirmDelete: true,
      deleteButtonContent: '<i class="ti-trash text-danger m-r-10"></i>',
      saveButtonContent: '<i class="ti-save text-success m-r-10"></i>',
      cancelButtonContent: '<i class="ti-close text-danger"></i>',
    },
  }

  @ViewChild('userTable') userTable: Ng2SmartTableComponent;

  constructor(private userService: UserService, private taskService: TaskService) { }

  ngAfterViewInit() {
    this.userService.getUsers().subscribe(
      data => {
        this.users = new LocalDataSource(data);
        setTimeout(() => {
          this.userTable.grid.dataSet.deselectAll();
          $($(`ng2-smart-table tbody tr`).first()[0]).trigger('click');
        })
      }
    )
  }

  // User Table Settings
  onUserSelected(event) {
    this.selectedUser = event.data
    this.getTasks(this.selectedUser.id)
  }
  onUserCreateConfirm(event) {
    const user = event.newData
    this.userService.createUser(user).subscribe(
      id => {
        event.newData.id = id
        event.confirm.resolve(event.newData);
      },
      err => {
        event.confirm.reject();
      }
    )
  }
  onUserSaveConfirm(event) {
    const user = event.newData
    this.userService.updateUser(user).subscribe(
      data => {
        event.confirm.resolve(event.newData);
      },
      err => {
        event.confirm.reject();
      }
    )
  }
  onUserDeleteConfirm(event) {
    this.userService.deleteUser(event.data['id']).subscribe(
      data => {
        event.confirm.resolve(event.newData);
      },
      err => {
        event.confirm.reject();
      }
    )
  }

  // Task Table Settings
  getTasks(userid) {
    this.taskService.getTasks(userid).subscribe(
      data => {
        this.tasks = new LocalDataSource(data);
      }
    )
  }
  onTaskSelected(event) {
    this.selectedTask = event.data
  }
  onTaskCreateConfirm(event) {
    const task = {...event.newData, userid: this.selectedUser.id}
    this.taskService.createTask(task).subscribe(
      id => {
        event.newData.id = id
        event.confirm.resolve(event.newData);
      },
      err => {
        event.confirm.reject();
      }
    )
  }
  onTaskSaveConfirm(event) {
    const task = event.newData
    this.taskService.updateTask(task).subscribe(
      data => {
        event.confirm.resolve(event.newData);
      },
      err => {
        event.confirm.reject();
      }
    )
  }
  onTaskDeleteConfirm(event) {
    this.userService.deleteUser(event.data['id']).subscribe(
      data => {
        event.confirm.resolve(event.newData);
      },
      err => {
        event.confirm.reject();
      }
    )
  }
}