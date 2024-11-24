import { observer } from 'mobx-react-lite';
import { useEffect, useRef, useState } from 'react';
import PopUpWindow from '../../../shared/popup-windows/pop-up-window/PopUpWindow';
import styles from './main.module.css';
import UserState from '../../../../../state/entities/UserState';
import Base64Handler from '../../../../../utils/handlers/Base64Handler';
import image from './images/image.png';
import smile from './images/smile.png';
import Attachments from '../../../ui-kit/attachments/Attachments';
import FilesInputWrapper from '../../wrappers/files-input-wrapper/FilesInputWrapper';

const CreatePublicationPopup = observer(({isOpen = false, close = () => {}}) => {
    const [text, setText] = useState('');
    const [attachments, setAttachments] = useState([]);
    const textareaRef = useRef(null);
  
    useEffect(() => {
        if (textareaRef.current) {
            const numberOfLines = text.split('\n').length;
            const textAreaHeight = numberOfLines * 25;
    
            textareaRef.current.style.height = 'auto';
            textareaRef.current.style.height = `${textAreaHeight}px`;
        }
    }, [text]);
  
    const textareaChangeHandler = (event) => {
        setText(event.target.value);
    };

    return (
        <PopUpWindow open={isOpen} title={'New publication'} close={close}>
            <div className={styles.wrapper}>
                <div className={styles.content}>
                    <div className={styles.header}>
                        <img 
                            src={Base64Handler.Base64ToUrlFormatPng(UserState.user.avatar)} 
                            className={styles.avatar} 
                            draggable="false"
                        />
                        <div className={styles.information}>
                            <span className={styles.nickname}>{UserState.user.nickname}</span>
                            <span className={styles.name}>{UserState.user.name} {UserState.user.surname}</span>
                        </div>
                    </div>
                    <textarea 
                        ref={textareaRef}
                        placeholder='Write a publication'
                        className={styles.textarea}
                        autoFocus
                        onChange={textareaChangeHandler}
                    ></textarea>
                    <div className={styles.attachments}>
                        <Attachments attachments={attachments} />
                    </div>
                </div>
                <div className={styles.bottom}>
                    <div className={styles.controlAttachments}>
                        <span>Attach something to your post</span>
                        <div className={styles.control}>
                            <FilesInputWrapper setFiles={setAttachments} maxLength={7}>
                                <img src={image} draggable="false" />
                            </FilesInputWrapper>
                            {/* <img 
                                src={smile} 
                                draggable="false" 
                            /> */}
                        </div>
                    </div>
                    <button>Post</button>
                </div>
            </div>
        </PopUpWindow>
    );
});

export default CreatePublicationPopup;