import { IIngredient } from './ingredient';
import { IPhoto } from './photo';

export interface IRecipe {
    id: number;
    name: string;
    preparationTime: number;
    numberOfCalories: number;
    description?: string;
    photoUrl: string;
    categoryId: number;
    authorId: number;
    kitchenOriginId: number;
    dateAdded: Date;
    recipePhotos?: IPhoto[];
    ingredients: IIngredient[];
}