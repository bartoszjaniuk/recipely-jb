import { IRecipe } from './recipe';

export interface IKitchenOrigin {
    id: number;
    name: string;
    recipes?: IRecipe[];
}