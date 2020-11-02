import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, Resolve, RouterStateSnapshot } from '@angular/router';
import { Observable } from 'rxjs';
import { MembersService } from '../members/members.service';
import { IMember } from '../_models/member';

@Injectable({
    providedIn: 'root'
})
export class MemberDetailResolver implements Resolve<IMember> {
    
    constructor(private memberService: MembersService) {}
    
    resolve(route: ActivatedRouteSnapshot): Observable<IMember>  {
        return this.memberService.getMember(route.paramMap.get('username'));
    }

}