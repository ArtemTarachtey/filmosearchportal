export const getData = async (url: string, token: string) => {
    try {
        const response = await fetch(url, {
            headers: {
                'Authorization': `Bearer ${token}`,
                'Content-Type': 'application/json'
            }
        })
        if (!response.ok) throw new Error("Error...")
        const data = await response.json();
            return data;
        } catch (error) {
        console.error(error);
            return null;
    }
}

export const updateData = async (url: string, token: string, bodyData: string) => {
     try {
        const response = await fetch(url, {
            method: 'PUT',
            headers: {
                'Authorization': `Bearer ${token}`,
                'Content-Type': 'application/json'
            },
            body: bodyData,
        })
        if (!response.ok) throw new Error("Error...")
            return response.ok;
        } catch (error) {
            console.error(error);
            return false;
    }
}

export const deleteData = async (url: string, token: string, id: string) => {
    try {
        const response = await fetch(`${url}/${id}`, {
            method: 'DELETE',
            headers: {
                'Authorization': `Bearer ${token}`,
            },
        })
        if (!response.ok) throw new Error("Delete failed...")
            return response.ok;
        } catch (error) {
            console.error(error);
            return false;
    }
}

export const createData = async (url: string, token: string, bodyData: string) => {
    try {
        const response = await fetch(url, {
            method: 'POST',
            headers: {
                'Authorization': `Bearer ${token}`,
                'Content-Type': 'application/json'
            },
            body: bodyData
        })
        if (response.ok) return await response.text();
        throw new Error(`Ошибка сервера: ${response.status}`);
    } 
    catch (error) {
        console.error(error);
        return null;
    }
}


export const handleSend = async <T extends { id?: string }> (url: string, dto: unknown, entity: T, setItems: React.Dispatch<React.SetStateAction<T[]>>, dialogMode: string, token: string, dialogRef: React.RefObject<HTMLDialogElement | null>) => {
    try {
        const bodyData = JSON.stringify(dto)
        if (dialogMode == 'Создание') {
        const newIdRaw = await createData(url, token, bodyData);
        if (newIdRaw) {
            const newId = typeof newIdRaw === 'string' ? newIdRaw.replace(/"/g, '') : newIdRaw;
            const newReview = { ...entity, id: newId };
            setItems(prev => [...prev, newReview]); 
        }
        } else {
            const isSuccess = await updateData(url, token, bodyData);
            if (isSuccess) setItems(prev => prev.map(r => r.id == entity.id ? entity : r))
        }  
        dialogRef.current?.close(); 
    }
    catch (error) {
        console.error(error);
    }
}  


export const handleDelete = async <T extends {id?: string}> (url: string, id: string, token: string, setItems: React.Dispatch<React.SetStateAction<T[]>>, dialog: React.RefObject<HTMLDialogElement | null>) => {
    const isDelete = await deleteData(url, token, id);
    if (isDelete) {
        setItems(prev => prev.filter(a => a.id != id));
        dialog.current?.close()
    }
}