KEYCLOAK_BIN=/opt/keycloak/bin/kcadm.sh

set -a
source /opt/keycloak/.env
set +a

$KEYCLOAK_BIN config credentials --server http://keycloak:$KEYCLOAK_PORT --realm master --user $KEYCLOAK_ADMIN_LOGIN --password $KEYCLOAK_ADMIN_PASSWORD

$KEYCLOAK_BIN create realms -s realm=$REALM -s enabled=true

echo '{
  "clientId": "'"$CLIENT_ID"'",
  "secret": "'"$CLIENT_SECRET"'",
  "enabled": true,
  "protocol": "openid-connect",
  "publicClient": false,
  "directAccessGrantsEnabled": true,
  "rootUrl": "http://localhost:'"$ACCOUNT_SERVICE_API_PORT"'",
  "baseUrl": "/",
  "redirectUris": ["http://localhost/*", "http://localhost:'"$ACCOUNT_SERVICE_API_PORT"'/*"],
  "webOrigins": ["http://localhost", "http://localhost:'"$ACCOUNT_SERVICE_API_PORT"'"],
  "attributes": {
    "post.logout.redirect.uris": "http://localhost:'"$ACCOUNT_SERVICE_API_PORT"'"
  }
}' | $KEYCLOAK_BIN create clients -r $REALM -f -

$KEYCLOAK_BIN create roles -r $REALM -s name=offline_access
$KEYCLOAK_BIN create roles -r $REALM -s name=uma_authorization

$KEYCLOAK_BIN create users \
  -r $REALM \
  -s username=$USERNAME \
  -s email=test@example.com \
  -s firstName=testFirstname \
  -s lastName=testLastname \
  -s enabled=true

USER_ID=$($KEYCLOAK_BIN get users -r $REALM -q username=$USERNAME --fields id --format csv | tail -n1 | tr -d '"')

$KEYCLOAK_BIN set-password -r $REALM --userid $USER_ID --new-password $PASSWORD

$KEYCLOAK_BIN add-roles -r $REALM --uusername $USERNAME --rolename offline_access
$KEYCLOAK_BIN add-roles -r $REALM --uusername $USERNAME --rolename uma_authorization
