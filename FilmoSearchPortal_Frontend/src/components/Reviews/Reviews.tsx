import { useAuth0 } from '@auth0/auth0-react';
import { useEffect, useState, useRef } from "react";
import { getData, handleDelete, handleSend } from '../../utils/apiService';
import { type IReview } from '../../types/IReview';
import { type IFilmShort, type IFilmResponse } from '../../types/IFilm';
import { FILM_URL, REVIEW_URL } from '../../utils/constants';

const EMPTY_REVIEW = { id: '', title: '', text: '', stars: 0, film: { filmId: '', filmTitle: ''} }

function Reviews() {
    const { getAccessTokenSilently, isLoading } = useAuth0();
    const errorDialog = useRef<HTMLDialogElement>(null);
    const [dialogMode, setDialogMode] = useState<string | null>(null);
    const dialogRef = useRef<HTMLDialogElement>(null);
    const confirmDialog = useRef<HTMLDialogElement>(null);
    const [deletedReview, setDeletedReview] = useState<IReview | null>(null);
    const [selectedReview, setSelectedReview] = useState<IReview | null>(null);
    const [reviews, setReviews] = useState<IReview[]>([]);
    const [films, setFilms] = useState<IFilmShort[]>([]);

    useEffect(() => {
        const loadFilms = async () => {
            const token = await getAccessTokenSilently();
            const data = await getData(FILM_URL, token);
            if (data) {
                const shortFilms = data.map((f: IFilmResponse) => 
                    ({
                        filmId: f.id,
                        filmTitle: f.title
                    })
                );
                setFilms(shortFilms);
            }
        }
        loadFilms();
    }, [getAccessTokenSilently, isLoading])

    useEffect(() => {
        const loadReviews = async () => {
            const token = await getAccessTokenSilently();
            try {
                const data = await getData(REVIEW_URL, token);
                if (data) setReviews(data);
            } catch (error) {
                console.error(error);
            }
        }
        loadReviews();
    }, [getAccessTokenSilently, isLoading]) 

    const handleEditClick = (r: IReview) => {
            setDialogMode('Редактирование');
            setSelectedReview(r);
            dialogRef.current?.showModal();
        }
    
        const handleDeleteClick = (r: IReview) => {
            setDeletedReview(r);
            confirmDialog.current?.showModal();
        }
    
        const handleCreateClick = () => {
            setDialogMode('Создание');
            setSelectedReview(EMPTY_REVIEW);
            dialogRef.current?.showModal();
        }
    
        const sendData = async (e: React.FormEvent) => {
            e.preventDefault()
            if (!selectedReview) return;
            const isValid = selectedReview.stars >= 0 && selectedReview.stars <= 5;
            if (!isValid) {
                errorDialog.current?.showModal();
                return;
            } else {
                const token = await getAccessTokenSilently();
                const reviewDto = {
                    id: selectedReview.id,
                    title: selectedReview.title,
                    text: selectedReview.text,
                    stars: selectedReview.stars,
                    filmId: selectedReview.film?.filmId 
                };
                console.log(selectedReview)
                await handleSend(REVIEW_URL, reviewDto, selectedReview, setReviews, dialogMode!, token, dialogRef);
            }
        }  

    const deleteFilmData = async () => {
        if (!deletedReview || !deletedReview.id) return;
        const token = await getAccessTokenSilently();
        await handleDelete(REVIEW_URL, deletedReview.id, token, setReviews, confirmDialog);
    }


    if (isLoading) return ( <div>Загрузка...</div> )

    return (
        <>
            <h1>Обзоры!</h1>
            <div className='container'>
                { reviews ? reviews.map(r => 
                <div className='card' key={r.id}>
                    <div className='card-field title'>{r.title}</div>
                    <div className='card-field film_title'>{r.film.filmTitle}</div>
                    <div className='card-field description'>{r.text}</div>
                    <div className='card-field stars'>{r.stars}</div>
                    <div className='card-actions'>
                        <button onClick={() => handleEditClick(r)} className='edit-action'></button>
                        <button onClick={() => handleDeleteClick(r)} className='delete-action'></button>
                    </div>
                </div>
                ) : <div>Error...</div>}
            </div>
            <dialog ref={errorDialog} className='modal'>
                <h2>Ошибка!</h2>
                <p>Рейтинг должен быть от 1 до 5 звезд.</p>
                <button className='btn' type="button" onClick={() => errorDialog.current?.close()}>Отмена</button>
            </dialog>
            <dialog ref={confirmDialog} className='modal delete-modal'>
                <h2>Вы уверены что хотите удалить данный обзор?</h2>
                <div className="modal-buttons">
                    <button className='btn' type="submit" onClick={() => deleteFilmData()}>Удалить</button>
                    <button className='btn' type="button" onClick={() => confirmDialog.current?.close()}>Отмена</button>
                </div>
            </dialog>
            <dialog ref={dialogRef} className="modal">
                {selectedReview && (
                 <form className="form" method="dialog" onSubmit={async (e) => sendData(e) }>
                        <h2>{dialogMode}</h2>
                        <label className='form-label' htmlFor="input-title">Заголовок:</label>
                        <input
                            id='input-title'
                            className='form-input'
                            type='text' 
                            value={selectedReview?.title} 
                            onChange={(e) => setSelectedReview({...selectedReview, title: e.target.value})}
                        />
                        
                        <label className='form-label' htmlFor="input-desc">Текст:</label>
                        <textarea 
                            id="input-desc"
                            className='form-input'
                            rows={5}
                            value={selectedReview.text}
                            onChange={(e) => setSelectedReview({...selectedReview, text: e.target.value})}
                        />

                        <label className='form-label' htmlFor="input-duration">Звезды:</label>
                        <input 
                            id='input-duration'
                            className='form-input'
                            type='number' 
                            value={selectedReview.stars}
                            onChange={(e) => { const val = e.target.value; setSelectedReview({...selectedReview, stars: val == "" ? 0 : parseInt(val)})}}
                        />

                        <label className='form-label' htmlFor="input-film">Фильм:</label>
                        <select 
                            id='input-film'
                            className='form-input'
                            value={selectedReview.film.filmId}
                            onChange={(e) => {
                                const selectedIds = films.find(f => f.filmId === e.target.value);
                                if (selectedIds) setSelectedReview({ ...selectedReview, film: { filmId: selectedIds.filmId, filmTitle: selectedIds.filmTitle} });
                            }}
                        >
                            {films.map(f => (
                                <option key={f.filmId} value={f.filmId}>
                                    {f.filmTitle}
                                </option>
                            ))}
                        </select>
                        <div className="modal-buttons">
                            <button className='btn' type="submit">Сохранить</button>
                            <button className='btn' type="button" onClick={() => dialogRef.current?.close()}>Отмена</button>
                        </div>
                    </form>
                )}
            </dialog>
            <button className='add-btn' onClick={() => handleCreateClick()}>+</button>
        </>
    )
}

export default Reviews;