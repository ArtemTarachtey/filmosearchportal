export interface IReview {
    id?: string;
    title: string;
    text: string;
    stars: number;
    film: { filmId: string, filmTitle: string };
}
