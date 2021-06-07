import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { UserDto } from './_models/userdto';
import { AccountService } from './_services/account.service';


@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  title = 'client';
  //users : any; //This functionality is implemented inside Register component
  constructor(private objHttp: HttpClient, private objAccServ:AccountService)
  {

  }
  ngOnInit():void{
    console.log("1 App component is loading");
    //this.getUsers();
    this.setCurrentUser();
  }
  setCurrentUser( ){
    console.log("app.compnent => inside setCurrentUser credential");
    const objUser: UserDto = JSON.parse(localStorage.getItem('user')!);
    this.objAccServ.setCurrentUser(objUser);
    
  }
  /*
  //This functionality is implemented inside Register componentng
  getUsers():void{
    this.objHttp.get('https://localhost:5001/api/users/')
    .subscribe(
              response=>{
                this.users = response;
              },
              error=>{
                console.log("Error generated at calling USERS API");
              });
  }*/
}
