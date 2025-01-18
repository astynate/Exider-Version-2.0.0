import ApplicationState from "../../../state/application/ApplicationState";
import { instance } from "../../../state/application/Interceptors";
import StorageState from "../../../state/entities/StorageState";

class FilesController {
    static GetFilesByParentCollectionId = async (id, onSuccess = () => {}) => {
        const length = StorageState.files[id] ? StorageState.files[id].items.length : 0;

        await instance
            .get(`api/files?id=${id ?? ''}&skip=${length}&take=${5}`)
            .then(response => {
                if (response.data && response.data.length) {
                    onSuccess(response.data);
                }
            })
            .catch((error) => { 
                ApplicationState.AddErrorInQueueByError('Attention!', error);
            });
    };

    static RenameFile = async (name, id) => {    
        await instance
            .put(`/api/files?id=${id}&name=${name}`)
            .catch((error) => {
                ApplicationState.AddErrorInQueueByError('Attention!', error);
            });
    }

    static Delete = async (ids) => {
        for (let i = 0; i < ids.length; i++) {
            await instance
                .delete(`/api/files?id=${ids[i]}`)
                .catch((error) => {
                    ApplicationState.AddErrorInQueueByError('Attention!', error);
                });
        }
    }
};

export default FilesController;