import React from 'react';
import styles from './main.module.css';
import PublicationsWrapper from '../../wrappers/publications-wrapper/PublicationsWrapper';
import Publication from '../../../components/publication/Publication';

const PublicationList = ({
        setLike = () => {}, 
        comments, 
        id, 
        setUploadingComment, 
        deleteCallback, 
        isPublicationAvailable = true, 
        isPublications = false,
        type = 0
    }) => {

    return (
        <PublicationsWrapper>
            <div className={styles.publications}>
                <Publication />
                <Publication />
                <Publication />
                <Publication />
                <Publication />
                <Publication />
                <Publication />
                <Publication />
                <Publication />
                <Publication />
                <Publication />
                <Publication />
                <Publication />
                <Publication />
                <Publication />
                <Publication />
                <Publication />
                <Publication />
                {/* {isPublicationAvailable && 
                    <CommentField 
                        id={id} 
                        isPublications={isPublications}
                        setUploadingComment={setUploadingComment.bind(this)}
                    />} */}
                {/* {comments && comments.map && comments.map((element, index) => {
                    if (element.isUploading) {
                        return (
                            <Comment 
                                key={element.comment.id + "comment"}
                                isUploading={true}
                                comment={element.comment} 
                                user={element.user} 
                            />
                        )
                    } else {
                        return (
                            <Comment 
                                key={element.comment.id + "comment"}
                                comment={element.comment} 
                                user={element.user}
                                deleteCallback={deleteCallback}
                                setLike={setLike}
                            />
                        )
                    }
                })} */}
            </div>
        </PublicationsWrapper>
    );
 };

export default PublicationList;