import React, {ReactNode, Suspense} from 'react';
import {Canvas} from '@react-three/fiber';
import {GizmoHelper, GizmoViewport, Html, OrbitControls, useProgress} from '@react-three/drei';


interface SceneProps {
    children: ReactNode;  // Accepts any React component as children
}

//style={{ width: '100%', height: '90vh' }}

/**
 * Scene component for rendering a 3D scene using React Three Fiber.
 * It sets up a basic scene including lights, camera, controls, and helpers.
 * It also includes a loader that displays the progress of loading assets.
 *
 * @param {SceneProps} props - Properties passed to the Scene component.
 * @param {React.ReactNode} props.children - Children components to be rendered inside the scene.
 * @returns {React.FC<SceneProps>} A functional component that renders a 3D scene.
 */
const Scene: React.FC<SceneProps> = ({children}) => {

    /**
     * Loader is a functional component that displays the current loading progress as a percentage.
     *
     * @return {JSX.Element} A center-aligned element showing the loading progress percentage.
     */
    function Loader() {
        const {progress} = useProgress()
        return <Html center>{progress} % loaded</Html>
    }

// position={[0, 1, 5]} <PerspectiveCamera makeDefault fov={75}/>
    return (
        <Canvas shadows camera={{position: [-10, 10, 10], fov: 20}}>
            <ambientLight intensity={1}/>
            <directionalLight castShadow position={[-2.5, 5, -5]} intensity={0.5} shadow-mapSize={[1024, 1024]}/>
            <directionalLight castShadow position={[2.5, 5, 5]} intensity={1.5} shadow-mapSize={[1024, 1024]}>
                <orthographicCamera attach={'shadow-camera'} args={[-5, 5, 5, -5, 1, 50]}/>
            </directionalLight>
            <GizmoHelper alignment="bottom-right" margin={[100, 100]}>
                <GizmoViewport labelColor="white" axisHeadScale={1}/>
            </GizmoHelper>
            <gridHelper args={[20, 20]}/>
            <OrbitControls enableZoom={true}/>
            <Suspense fallback={<Loader/>}>
                {children}
            </Suspense>
        </Canvas>
    );
};


export default Scene;
