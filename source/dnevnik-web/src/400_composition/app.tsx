import s from './styles/app.module.scss'
import { params } from '../_shared/params';


export function App() {
  return (
    <div className={s.app}>
      url: { params.UsersURL }
    </div>
  )
}
