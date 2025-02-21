/**
 * The UploadProps interface defines the properties required for the file upload component.
 *
 * @interface UploadProps
 *
 * @property {() => void} onUploadSuccess - This callback function is triggered when the file upload is successful.
 */
export interface UploadProps {
    onUploadSuccess: () => void;  // Callback function for successful upload
}