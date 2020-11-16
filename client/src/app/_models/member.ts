import { ILike } from './like';
import { IPhoto } from './photo';
import { IRecipe } from './recipe';

export interface IMember {
    id: number;
    username: string;
    knownAs: string;
    age: number;
    gender: string;
    created: Date;
    lastActive: Date;
    photoUrl: string;
    city: string;
    country: string;
    introduction?: string;
    userPhotos?: IPhoto[];
    recipes: IRecipe[];
    likedByUsers?: ILike[];
    likedUsers?: ILike[];


}