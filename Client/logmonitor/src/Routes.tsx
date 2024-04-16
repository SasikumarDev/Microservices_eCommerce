import { createBrowserRouter } from 'react-router-dom';
import Deafult from './Pages/Deafult';

export const Routes = createBrowserRouter([
    {
        path: '',
        element: <Deafult />,
    }
])