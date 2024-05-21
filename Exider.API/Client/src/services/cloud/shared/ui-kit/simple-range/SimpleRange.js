import { React } from 'react';
import { Range } from 'react-range';
import styles from './main.module.css';

const SimpleRange = ({step, minValue, maxValue, value, setValue, loadPercentage}) => {
    return (
        <Range
            step={step}
            min={minValue}
            max={maxValue}
            values={value < maxValue ? [value] : [maxValue - 1]}
            onChange={(values) => setValue(values[0])}
            renderTrack={({ props, children }) => (
                <>
                    <div
                        {...props}
                        style={{
                            ...props.style,
                            height: '4px',
                            width: '100%',
                            overflow: 'visible',
                            position: 'relative',                
                            backgroundColor: 'var(--main-accent-color)',
                            cursor: 'pointer'
                        }}
                    >
                    <div 
                        style={{
                            ...props.style,
                            height: '4px',
                            position: 'absolute',
                            top: '0',
                            left: '0',
                            width: `${value / maxValue * 100}%`,
                            overflow: 'visible',
                            backgroundColor: 'var(--main-font-color)',
                            cursor: 'pointer'
                        }}
                    >
                    </div>
                    {loadPercentage &&
                        <div 
                            style={{
                                ...props.style,
                                height: '4px',
                                position: 'absolute',
                                top: '0',
                                left: '0',
                                width: `${loadPercentage}%`,
                                overflow: 'visible',
                                backgroundColor: 'var(--main-font-color)',
                                opacity: '0.34',
                                cursor: 'pointer'
                            }}
                        >
                        </div>
                    }
                        {children}
                    </div>
                </>
            )}
            renderThumb={({ props }) => (
                <div
                    {...props}
                    className={styles.thumb}
                    style={{
                        ...props.style,
                        cursor: 'pointer'
                    }}
                />
            )}
        />
    );
}

export default SimpleRange;