import { Injectable } from '@angular/core';
import { CanActivate} from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Observable, of as observableOf } from 'rxjs';
import { map } from 'rxjs/operators';
import { AccountService } from '../_services/account.service';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {
  res: boolean=true;
   constructor(private objAccSrv:AccountService, private objToastr:ToastrService){}

  canActivate(): Observable<any> {
    return this.objAccSrv.recentUser$.pipe(
      map(user=>{
       if(user) return true;
       this.objToastr.error('You shall not pass!');
       return false;
      })
    )
    }
 /*
 following is the template of canActivate() generated by framework.
 here, we need the simplest form hence remove the unnecessary stuff.
  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {
    return true;
  }
  */
}
