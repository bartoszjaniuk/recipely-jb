import { IUserPhoto } from './userPhoto';

export interface IMember {
    id: number;
    username: string;
    knownAs: string;
    age: number;
    gender: string;
    created: Date;
    lastActive: any;
    photoUrl: string;
    city: string;
    country: string;
    introduction?: string;
    userPhotos?: IUserPhoto[];
}