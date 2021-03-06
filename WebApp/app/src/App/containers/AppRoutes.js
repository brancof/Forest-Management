import React from 'react';
import {
  Switch,
  Route,
  Redirect
} from "react-router-dom";
import Login from './Login';
import Register from './Register';
import Proprietarios from './Proprietarios';
import Inspetores from './Inspetores';
import Trabalhadores from './Trabalhadores';
import Supervisores from './Supervisores';
import RecuperarPass from './RecuperarPass';
import Navbar from './Navbar';
import './AppRoutes.css';


class AppRoutes extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
      username: '',
      token: '',
      accounttype: 'proprietarios',
      user: '',
      notifier: 0,
      pwmessage: false
    };

    this.setUsername = this.setUsername.bind(this);
    this.setType = this.setType.bind(this);
    this.setUser = this.setUser.bind(this);
    this.setToken = this.setToken.bind(this);
    this.setPwmessage = this.setPwmessage.bind(this);
  }

  setUsername(newusername) {
    this.setState({ username: newusername });
  }

  setToken(newtoken) {
    this.setState({ token: newtoken });
  }

  setPwmessage(value) {
    this.setState({ pwmessage: value });
  }

  setUser(newuser) {
    this.setState({ user: newuser });
  }

  setType(newtype) {
    this.setState({ accounttype: newtype });
  }

  notifUpdater = () => {
    this.setState({ notifier: this.state.notifier++});
  };

  render() {

    return (
      <div className="AppRouter">
        {!(this.state.username === '') ? <Navbar user={this.state.user} token={this.state.token} accounttype={this.state.accounttype} notify={this.state.notifier}
          change={{
            username: this.setUsername,
            accounttype: this.setType,
            user: this.setUser,
            token: this.setToken
          }} />
          : null}
          <Switch>
            <Route path="/login">
              {this.state.username === '' ? <Login pwinfo={{show: this.state.pwmessage, set: this.setPwmessage}} 
              change={{
                username: this.setUsername,
                accounttype: this.setType,
                user: this.setUser,
                token: this.setToken
              }} />
                : <Redirect to="/" />}
            </Route>

            <Route path="/registo">
              <Register />
            </Route>

            <Route path="/proprietarios">
              {!(this.state.username === '') ? <Proprietarios user={this.state.user} username={this.state.username} accounttype={this.state.accounttype} change={{ user: this.setUser }} token={this.state.token} updateNotifs={this.notifUpdater} />
                : <Redirect to="/" />}
            </Route>

            <Route path="/inspetores">
              {!(this.state.username === '') ? <Inspetores username={this.state.username} user={this.state.user} accounttype={this.state.accounttype} token={this.state.token} updateNotifs={this.notifUpdater} />
                : <Redirect to="/" />}
            </Route>

            <Route path="/trabalhadores">
              {!(this.state.username === '') ? <Trabalhadores user={this.state.user} username={this.state.username} accounttype={this.state.accounttype} token={this.state.token} updateNotifs={this.notifUpdater} />
                : <Redirect to="/" />}
            </Route>

            <Route path="/supervisores">
              {!(this.state.username === '') ? <Supervisores username={this.state.username} user={this.state.user} accounttype={this.state.accounttype} token={this.state.token} updateNotifs={this.notifUpdater} />
                : <Redirect to="/" />}
            </Route>

            <Route path="/recuperar"> <RecuperarPass  pwinfo={{show: this.state.pwmessage, set: this.setPwmessage}}/> </Route>

            <Route exact path="/">
              {this.state.username === '' ? <Redirect to="/login" /> : <Redirect to={"/" + this.state.accounttype} />}
            </Route>
          </Switch>
      </div>
    );
  }
}

export default AppRoutes;